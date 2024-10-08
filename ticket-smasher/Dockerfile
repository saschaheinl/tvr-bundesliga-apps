# Stage 1: Build the Quasar app
FROM node:lts-alpine as build-stage
WORKDIR /app

# Copy the .env file to the container
COPY .env .env

# Install dependencies
COPY package*.json ./
RUN npm install

# Copy all other files
COPY . .

# Ensure that the environment variables are loaded and available
RUN echo "Environment Variables:" && cat .env && export $(cat .env | xargs) && env && npm run build

# Stage 2: Serve the app using Nginx
FROM nginx:stable-alpine as production-stage
COPY --from=build-stage /app/dist/spa /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf 
EXPOSE 8080  

CMD ["nginx", "-g", "daemon off;"]
