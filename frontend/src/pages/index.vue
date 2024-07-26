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
          <VBtn @click="updateTask(task.id, task.isComplete)">{{
            task.isComplete ? "Mark Uncomplete" : "Mark Complete"
          }}</VBtn>
        </VCardActions>
      </VCard>
    </VSheet>
    <div class="d-flex mt-5">
      <VBtn height="50" class="flex-1-0 mr-5" to="/create">Create Task</VBtn>
      <VBtn
        height="50"
        class="flex-1-1"
        @click="includeCompleted = !includeCompleted"
        >{{ includeCompleted ? "Show Uncompleted" : "Show Completed" }}
      </VBtn>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { useQuery, useMutation } from "@vue/apollo-composable";
import gql from "graphql-tag";
import { ref, watch, computed } from "vue";

const includeCompleted = ref(false);
const query = computed(() =>
  includeCompleted.value ? {} : { isComplete: { eq: false } }
);

const { result, loading, refetch } = useQuery(
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

const { mutate: mutateTask } = useMutation(gql`
  mutation updateTask($id: String!, $isComplete: Boolean!) {
    updateTask(input: { id: $id, isComplete: $isComplete }) {
      task {
        id
      }
    }
  }
`);

watch(includeCompleted, () => {
  loading.value = true;
  refetch();
});

const updateTask = async (id: string, isComplete: boolean) => {
  await mutateTask({
    id,
    isComplete: !isComplete,
  });
  loading.value = true;
  refetch();
};
</script>
