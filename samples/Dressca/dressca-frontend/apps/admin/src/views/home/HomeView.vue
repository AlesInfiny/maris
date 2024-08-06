<script setup lang="ts"></script>

<template>
  <div
    class="container mx-auto flex flex-col items-center justify-center gap-6"
  >
    <div class="text-5xl font-bold">Dressca 管理 トップ</div>
    <table class="table-auto border-separate">
      <thead>
        <tr>
          <th colspan="2" class="border bg-light-blue-800 text-3xl text-white">
            メニュー
          </th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td class="border bg-light-blue-50 text-3xl underline">
            <router-link to="catalog">カタログアイテム管理</router-link>
          </td>
          <td class="border bg-light-blue-50 text-3xl ">
            カタログアイテムの一覧を表示します。
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
