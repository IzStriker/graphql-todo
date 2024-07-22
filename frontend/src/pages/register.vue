<template>
  <v-flex class="w-50">
    <VCardTitle class="text-h4">Register</VCardTitle>
    <VForm @submit.prevent="submit">
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
      <VTextField
        v-model="formData.confirmPassword"
        label="Confirm Password"
        type="password"
        :error="formErrors.confirmPassword"
        :error-messages="errorMessages.confirmPassword"
      ></VTextField>
      <VAlert type="error" class="mb-5" v-if="errorMessages.form">{{
        errorMessages.form
      }}</VAlert>
      <VBtn type="submit" block class="mb-5">Register</VBtn>
      <VBtn type="button" to="/login" block> Already have an Account </VBtn>
    </VForm>
  </v-flex>
</template>
<script lang="ts" setup>
import { ref } from "vue";
import { useMutation } from "@vue/apollo-composable";
import gql from "graphql-tag";
import { toast } from "vue3-toastify";
import { useRouter } from "vue-router";

const router = useRouter();
const formData = ref<Record<string, string>>({
  email: "",
  password: "",
  confirmPassword: "",
});
const formErrors = ref<Record<string, boolean>>({
  email: false,
  password: false,
  confirmPassword: false,
});
const errorMessages = ref<Record<string, string | undefined>>({
  form: undefined as string | undefined,
  email: undefined as string | undefined,
  password: undefined as string | undefined,
  confirmPassword: undefined as string | undefined,
});

const { mutate } = useMutation(gql`
  mutation register($email: String!, $password: String!) {
    registerUser(input: { email: $email, password: $password }) {
      user {
        id
        email
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
  ["email", "password", "confirmPassword"].forEach((f: string) => {
    formErrors.value[f] = false;
    errorMessages.value[f] = undefined;
    if (formData.value[f] == null || formData.value[f].trim().length == 0) {
      formErrors.value[f] = true;
      errorMessages.value[f] = "Field Required";
    }
  });
  if (formData.value.password !== formData.value.confirmPassword) {
    formErrors.value.password = true;
    formErrors.value.confirmPassword = true;
    errorMessages.value.password = "Passwords don't match";
    errorMessages.value.confirmPassword = "Passwords don't match";
  }
  if (Object.values(formErrors.value).some((v) => v)) {
    return;
  }
  const res = await mutate({
    email: formData.value.email,
    password: formData.value.password,
  });
  if (res?.data.registerUser.errors) {
    res.data.registerUser.errors.forEach((e: any) => {
      if (
        e.__typename === "EmailInUseError" ||
        e.__typename === "InvalidEmailError"
      ) {
        formErrors.value.email = true;
        errorMessages.value.email = e.message;
      } else {
        errorMessages.value.form = e.message;
      }
    });
    return;
  }

  await router.push({ path: "/login" });
  toast("User Registered", {
    type: "success",
    position: "bottom-center",
    autoClose: 5000,
  });
};
</script>
