import { useUserStore } from '@/stores/user/user'
import { authenticationService } from '@/services/authentication/authentication-service'

export async function fetchUser() {
  const userStore = useUserStore()
  if (!authenticationService.isAuthenticated().value) {
    return
  }
  await userStore.fetchUserResponse()
}
