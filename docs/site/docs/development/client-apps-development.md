# Back Office Application

- At the root of `src/backoffice`, we create a `.env` with content as below

```
REACT_APP_GRAPHQL_ENDPOINT=http://localhost:5011
```

This will point to the local graphql endpoint. When we deploy it to production, we will ovewrite it with another configuration

- Then we run `yarn start` to start development the `back-office` app
