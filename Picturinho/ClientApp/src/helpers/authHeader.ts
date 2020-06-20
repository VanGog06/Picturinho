export const authHeader = () => {
  // return authorization header with jwt token
  const localStorageItem: string | null = localStorage.getItem("user");

  if (localStorageItem) {
    const user = JSON.parse(localStorageItem);

    if (user && user.token) {
      return { Authorization: "Bearer " + user.token };
    }
  }

  return null;
};
