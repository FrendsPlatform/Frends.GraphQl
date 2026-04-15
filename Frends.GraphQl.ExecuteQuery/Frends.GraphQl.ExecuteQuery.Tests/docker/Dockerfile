FROM node:14.16.0
WORKDIR /app
COPY index.js package.json package-lock.json  ./
COPY certs ./certs
EXPOSE 4000
EXPOSE 4001
RUN npm ci
CMD ["node", "index.js"]
