<script setup lang="ts"></script>

<template>
  <div
    class="container mx-auto flex flex-col items-center justify-center gap-6"
  >
    <div class="p-8 text-5xl font-bold">カタログ管理</div>
    <table class="border-slate-400 table-auto border-collapse">
      <tr>
        <th
          colspan="2"
          class="border-slate-300 border bg-blue-800 p-4 text-3xl text-white"
        >
          メニュー
        </th>
      </tr>
      <tr>
        <td class="border-slate-300 border bg-blue-50 p-8 text-3xl underline">
          <router-link to="catalog/items">カタログアイテム一覧</router-link>
        </td>
        <td class="border-slate-300 border bg-blue-50 text-3xl">
          カタログアイテムの一覧を表示します。
        </td>
      </tr>
      <tr>
        <td class="border-slate-300 border bg-blue-50 p-8 text-3xl underline">
          <router-link to="catalog/items/add">カタログアイテム追加</router-link>
        </td>
        <td class="border-slate-300 border bg-blue-50 text-3xl">
          カタログにアイテムを追加します。
        </td>
      </tr>
    </table>
  </div>
</template>
