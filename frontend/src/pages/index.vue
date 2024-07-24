<template>
  <div class="d-flex flex-column justify-center w-100 h-100">
    <VSheet max-height="500" class="h-75 w-100 overflow-auto">
      <VProgressCircular indeterminate v-if="loading" />
      <VCard
        v-for="task in result.tasks"
        :title="task.title"
        :text="task.description"
        variant="tonal"
        class="mx-5 my-5"
        :key="task.id"
        v-else
      >
        <VCardActions>
          <VBtn>Mark Complete</VBtn>
        </VCardActions>
      </VCard>
    </VSheet>
    <div class="d-flex mt-5">
      <VBtn height="50" class="flex-1-0 mr-5">Create Task</VBtn>
      <VBtn height="50" class="flex-1-1">Show Completed</VBtn>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { useQuery } from "@vue/apollo-composable";
import gql from "graphql-tag";
import { ref } from "vue";

const includeCompleted = ref(true);
const query = includeCompleted.value ? {} : { isComplete: { eq: false } };

const { result, loading } = useQuery(
  gql`
    query getTasks($query: TaskFilterInput!) {
      tasks(where: $query) {
        description
        id
        isComplete
        title
        userId
      }
    }
  `,
  {
    query,
  }
);
</script>
