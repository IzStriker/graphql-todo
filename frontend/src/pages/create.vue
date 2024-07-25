<template>
  <div class="d-flex flex-column justify-center w-100 h-100">
    <VCardTitle class="text-h4">Create Task</VCardTitle>
    <VForm @submit.prevent="submit">
      <VTextField
        label="Title"
        type="text"
        v-model="formData.title"
        :error="formErrors.title"
        :error-messages="errorMessages.title"
      ></VTextField>
      <VTextarea
        label="Description"
        v-model="formData.description"
        :error="formErrors.description"
        :error-messages="errorMessages.description"
      ></VTextarea>
      <VBtn type="submit" block class="mb-5">Create</VBtn>
      <VBtn type="button" block class="mb-5" to="/register"> View Tasks </VBtn>
    </VForm>
  </div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { useMutation } from "@vue/apollo-composable";
import { useRouter } from "vue-router";
import { toast } from "vue3-toastify";
import gql from "graphql-tag";

const router = useRouter();
const formData = ref<Record<string, string>>({
  title: "",
  description: "",
});
const formErrors = ref<Record<string, boolean>>({
  title: false,
  description: false,
});

const errorMessages = ref<Record<string, string | undefined>>({
  title: undefined as string | undefined,
  description: undefined as string | undefined,
});

const { mutate } = useMutation(gql`
  mutation createTask($title: String!, $description: String!) {
    createTask(
      input: { title: $title, description: $description, isComplete: false }
    ) {
      task {
        title
      }
    }
  }
`);

const submit = async () => {
  ["title", "description"].forEach((f: string) => {
    formErrors.value[f] = false;
    errorMessages.value[f] = undefined;

    if (formData.value[f] == null || formData.value[f].trim().length == 0) {
      formErrors.value[f] = true;
      errorMessages.value[f] = "Field Required";
    }
  });

  if (Object.values(formErrors.value).some((v) => v)) {
    return;
  }

  const res = await mutate({
    title: formData.value.title,
    description: formData.value.description,
  });

  if (!res) return;

  await router.push({ path: "/" });
  toast(`Task successfully created "${res.data.createTask.task.title}"`, {
    type: "success",
    position: "bottom-center",
    autoClose: 5000,
  });
};
</script>
