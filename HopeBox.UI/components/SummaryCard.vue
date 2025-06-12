<template>
  <div class="summary-card" :style="{ borderLeft: '4px solid ' + color }">
    <i :class="['fas', icon]"></i>
    <div>
      <h4>{{ title }}</h4>
      <p>{{ value }}</p>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    title: String,
    value: String,
    icon: String,
    color: String
  }
}
</script>

<style scoped>
.summary-card {
  display: flex;
  align-items: center;
  background: #1e1e2f;
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.3);
}
.summary-card i {
  font-size: 24px;
  margin-right: 15px;
  color: inherit;
}
.summary-card h4 {
  margin: 0;
  font-size: 14px;
  color: #aaa;
}
.summary-card p {
  margin: 0;
  font-size: 18px;
  font-weight: bold;
}
</style>
