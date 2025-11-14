const { ApolloServer, gql, AuthenticationError } = require('apollo-server');

const data = {
    "users": [
        {
            "id": 1,
            "name": "John",
            "surname": "Doe",
        },
        {
            "id": 2,
            "name": "Marry",
            "surname": "White",
        },
        {
            "id": 3,
            "name": "Marry",
            "surname": "Doe",
        },
    ]
};

const typeDefs = gql`
	type User {
		id: ID
        name: String
        surname: String
	}

	input UserFilter {
		surname: String
		name: String
	}

	type Query {
        users(surname: String): [User]
		usersByFilter(filter: UserFilter): [User]
	}
`;

const resolvers = {
    Query: {
        users: (_, args) => {
            if (args.surname) {
                return data.users.filter(user => user.surname === args.surname);
            }
            return data.users;
        },
		usersByFilter: (_, args) => {
			let filtered = data.users;
			if (args.filter?.surname) {
				filtered = filtered.filter(user => user.surname === args.filter.surname);
			}
			if (args.filter?.name) {
				filtered = filtered.filter(user => user.name === args.filter.name);
			}
			return filtered;
		}
    }
};

const sleep = ms => new Promise(resolve => setTimeout(resolve, ms));

const context = async ({ req }) => {
    await sleep(1000);
    const authHeader = req.headers.authorization || "";
    if (authHeader.startsWith("Bearer ")) {
        const token = authHeader.split(" ")[1];
        if (token !== "valid-oauth-token") {
            throw new AuthenticationError("Invalid Bearer token: " + token);
        }
    } else if (authHeader.startsWith("Basic ")) {
        const base64Credentials = authHeader.split(" ")[1];
        const credentials = Buffer.from(base64Credentials, "base64").toString("utf-8");
        const [username, password] = credentials.split(":");
        if (username !== "admin" || password !== "secret") {
            throw new AuthenticationError("Invalid Basic credentials. Username: " + username + " Password: " + password);
        }
    }

    const customHeader = req.headers?.foo;
    if (customHeader && customHeader !== "Bar") {
        throw new AuthenticationError("Invalid Custom header. Key: Foo, Value: " + customHeader);
    }
}

const server = new ApolloServer({ typeDefs, resolvers, context });
server.listen(4000);