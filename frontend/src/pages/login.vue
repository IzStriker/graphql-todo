<template>
  <v-flex class="w-50">
    <v-card-title class="text-h4">Login</v-card-title>
    <v-form @submit.prevent="submit" ref="form">
      <v-text-field v-model="email" label="Email" type="email"></v-text-field>
      <v-text-field
        v-model="password"
        label="Password"
        type="password"
      ></v-text-field>
      <v-btn type="submit" block>Login</v-btn>
    </v-form>
  </v-flex>
</template>
<script lang="ts" setup>
import { ref } from "vue";
import { useMutation } from "@vue/apollo-composable";
import gql from "graphql-tag";

const form = ref();
const email = ref("");
const password = ref("");

const { mutate } = useMutation(gql`
  mutation login($email: String!, $password: String!) {
    login(input: { email: $email, password: $password }) {
      loginRes {
        token
        validTill
      }
      errors {
        __typename
        ... on Error {
          message
        }
      }
    }
  }
`);

const submit = async (e: Event) => {
  const res = await mutate({
    email: email.value,
    password: password.value,
  });
  if (res && res.errors == null) {
    localStorage.setItem("token", res.data.login.loginRes.token);
  }
};
</script>
