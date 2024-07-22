import {
  ApolloClient,
  InMemoryCache,
  HttpLink,
  ApolloLink,
} from "@apollo/client/core";
import { onError } from "@apollo/client/link/error";
import router from "@/router";

const authLink = new ApolloLink((operation, forward) => {
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

const logoutLink = onError(({ graphQLErrors }) => {
  if (
    graphQLErrors?.some((e) => e.extensions["code"] === "AUTH_NOT_AUTHORIZED")
  ) {
    localStorage.removeItem("token");
  }
});

export const apolloClient = new ApolloClient({
  cache: new InMemoryCache(),
  link: ApolloLink.from([logoutLink, authLink, httpLink]),
});
