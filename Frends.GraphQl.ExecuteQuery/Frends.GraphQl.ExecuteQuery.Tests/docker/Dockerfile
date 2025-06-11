FROM node:14.16.0
WORKDIR /app
COPY index.js package.json package-lock.json  ./
EXPOSE 4000
RUN npm ci
CMD ["node", "index.js"]