<script setup lang="ts"></script>

<template>
  <div class="container mx-auto">
    <div>カタログ管理</div>
    <table class="table-auto border-collapse border-slate-400">
      <tr>
        <th>メニュー</th>
      </tr>
      <tr>
        <td>
          <router-link to="catalog/items">カタログアイテム一覧</router-link>
        </td>
        <td>カタログアイテムの一覧を表示します。</td>
      </tr>
      <tr>
        <td>
          <router-link to="catalog/items/add">カタログアイテム追加</router-link>
        </td>
        <td>カタログにアイテムを追加します。</td>
      </tr>
    </table>
  </div>
</template>
