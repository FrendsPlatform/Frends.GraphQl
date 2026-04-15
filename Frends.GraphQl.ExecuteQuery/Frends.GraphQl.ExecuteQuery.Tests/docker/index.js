const { ApolloServer, gql, AuthenticationError } = require('apollo-server');
const fs = require('fs');
const https = require('https');
const path = require('path');

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
};

const certDir = path.join(__dirname, 'certs');
const httpsOptions = {
    key: fs.readFileSync(path.join(certDir, 'server-key.pem')),
    cert: fs.readFileSync(path.join(certDir, 'server-cert.pem')),
    requestCert: true,
    rejectUnauthorized: false,
};

const start = async () => {
    const httpServer = new ApolloServer({ typeDefs, resolvers, context });
    await httpServer.listen(4000);

    const httpsServer = https.createServer(httpsOptions, (req, res) => {
        const clientCertificate = req.socket?.getPeerCertificate?.();
        const clientCertificateIsValid = !!clientCertificate && Object.keys(clientCertificate).length > 0 && clientCertificate.subject?.CN === 'frends-client-cert';

        if (!clientCertificateIsValid) {
            res.writeHead(401, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify({ errors: [{ message: 'Client certificate is missing or invalid.' }] }));
            return;
        }

        res.writeHead(200, { 'Content-Type': 'application/json' });
        res.end(JSON.stringify({
            data: {
                users: [
                    { name: 'John' },
                    { name: 'Marry' },
                ],
            },
        }));
    });

    await new Promise(resolve => httpsServer.listen(4001, resolve));
};

start();
