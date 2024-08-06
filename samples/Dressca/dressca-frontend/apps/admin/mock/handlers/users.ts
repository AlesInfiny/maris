import { HttpResponse, http } from 'msw';
import type { UserResponse } from '@/generated/api-client';

const user: UserResponse = {
  userName: 'admin@example.com',
  roles: ['Administrator'],
};

export const usersHandlers = [
  http.get('/api/users', () => {
    return HttpResponse.json(user, { status: 200 });
  }),
];
