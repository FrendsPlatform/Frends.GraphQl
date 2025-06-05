FROM node:14.16.0
EXPOSE 4000
COPY index.js package.json package-lock.json  app/
WORKDIR /app
RUN npm ci
CMD ["node", "index.js"]