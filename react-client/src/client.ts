import { ApolloClient, InMemoryCache, HttpLink } from '@apollo/client';

const httpUri = process.env.REACT_APP_SERVER_URL + '/graphql';

const httpLink = new HttpLink({
  uri: httpUri,
  credentials: 'include'
});

const inMemoryCache = new InMemoryCache();

export default new ApolloClient({
  link: httpLink,
  cache: inMemoryCache
});
