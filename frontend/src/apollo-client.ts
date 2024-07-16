import { ApolloClient, InMemoryCache, HttpLink } from "@apollo/client/core";
import { ApolloLink } from "@apollo/client/core";

const authMiddleware = new ApolloLink((operation, forward) => {
  const token = localStorage.getItem("token");
  operation.setContext({
    headers: {
      authorization: token ? `Bearer ${token}` : "",
    },
  });
  return forward(operation);
});

const httpLink = new HttpLink({
  uri: "http://localhost:5221/graphql/",
});

export const apolloClient = new ApolloClient({
  cache: new InMemoryCache(),
  link: authMiddleware.concat(httpLink),
});
