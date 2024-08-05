<script setup lang="ts"></script>

<template>
  <div
    class="container mx-auto flex flex-col items-center justify-center gap-6"
  >
    <div class="text-xl font-bold">カタログ管理</div>
    <table class="border-slate-400 table-auto border-collapse">
      <tr>
        <th
          colspan="2"
          class="border-slate-300 border bg-light-blue-800 text-lg text-white"
        >
          メニュー
        </th>
      </tr>
      <tr>
        <td
          class="border-slate-300 border bg-light-blue-50 text-base underline"
        >
          <router-link to="catalog/items">カタログアイテム一覧</router-link>
        </td>
        <td class="border-slate-300 border bg-light-blue-50">
          カタログアイテムの一覧を表示します。
        </td>
      </tr>
      <tr>
        <td
          class="border-slate-300 border bg-light-blue-50 text-base underline"
        >
          <router-link to="catalog/items/add">カタログアイテム追加</router-link>
        </td>
        <td class="border-slate-300 border bg-light-blue-50">
          カタログにアイテムを追加します。
        </td>
      </tr>
    </table>
  </div>
</template>
