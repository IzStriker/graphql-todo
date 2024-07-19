<template>
  <v-flex class="w-50">
    <VCardTitle class="text-h4">Login</VCardTitle>
    <VForm @submit.prevent="submit" ref="form">
      <VTextField
        v-model="formData.email"
        label="Email"
        type="email"
        :error="formErrors.email"
        :error-messages="errorMessages.email"
      ></VTextField>
      <VTextField
        v-model="formData.password"
        label="Password"
        type="password"
        :error="formErrors.password"
        :error-messages="errorMessages.password"
      ></VTextField>
      <VAlert type="error" v-if="errorMessages.form">{{
        errorMessages.form
      }}</VAlert>
      <VBtn type="submit" block class="mt-5">Login</VBtn>
    </VForm>
  </v-flex>
</template>
<script lang="ts" setup>
import { ref } from "vue";
import { useMutation } from "@vue/apollo-composable";
import gql from "graphql-tag";

const form = ref();
const formData = ref<Record<string, string>>({
  email: "",
  password: "",
});
const formErrors = ref<Record<string, boolean>>({
  email: false,
  password: false,
});

const errorMessages = ref<Record<string, string | undefined>>({
  form: undefined as string | undefined,
  email: undefined as string | undefined,
  password: undefined as string | undefined,
});

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

const submit = async () => {
  ["email", "password"].forEach((f: string) => {
    formErrors.value[f] = false;
    errorMessages.value[f] = undefined;

    if (formData.value[f] == null || formData.value[f].trim().length == 0) {
      formErrors.value[f] = true;
      errorMessages.value[f] = "Field Required";
    }
  });

  if (formData.value.email == null || formData.value.email.trim().length == 0) {
    formErrors.value.email = true;
  }

  if (Object.values(formErrors.value).some((v) => v)) {
    return;
  }

  const res = await mutate({
    email: formData.value.email,
    password: formData.value.password,
  });

  if (res?.data.login.errors) {
    console.log("errors", res);
    formErrors.value.email = true;
    formErrors.value.password = true;
    errorMessages.value.form = res.data.login.errors
      .map((e: any) => e.message)
      .join("\n");
    return;
  }
  if (res && res.data.login.errors == null) {
    localStorage.setItem("token", res.data.login.loginRes.token);
  }
};
</script>
