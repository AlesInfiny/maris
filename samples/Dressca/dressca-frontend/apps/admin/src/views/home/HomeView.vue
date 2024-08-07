<script setup lang="ts"></script>

<template>
  <div
    class="container mx-auto flex flex-col items-center justify-center gap-6"
  >
    <div class="text-5xl font-bold p-8">Dressca 管理 トップ</div>
    <table class="table-auto border-separate ,-">
      <thead>
        <tr>
          <th colspan="2" class="border bg-blue-800 text-3xl text-white p-4">
            メニュー
          </th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td class="border bg-blue-50 text-3xl underline p-8">
            <router-link to="catalog">カタログアイテム管理</router-link>
          </td>
          <td class="border bg-blue-50 text-3xl p-8">
            カタログアイテムの一覧を表示します。
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
