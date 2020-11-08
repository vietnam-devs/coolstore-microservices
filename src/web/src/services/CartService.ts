import axios, { AxiosResponse, AxiosRequestConfig } from "axios";

import AuthService from "./AuthService";
import { ICart } from "stores/types";

const apiUrl = `${process.env.REACT_APP_API}`;
const cartResourceUrl = "/cart/api/carts";

const getRequestOptions = (token: string): AxiosRequestConfig => {
  return {
    baseURL: apiUrl,
    data: {},
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  };
};

export const getCartForCurrentUser = async () => {
  const user = await AuthService.getUser();
  const response: AxiosResponse = await axios.get<ICart>(
    `${cartResourceUrl}`,
    getRequestOptions(user.access_token)
  );
  console.log(response);
  return response.data as ICart;
};

export const createCartForCurrentUser = async (productId: string) => {
  const user = await AuthService.getUser();
  const response: AxiosResponse = await axios.post<ICart>(
    `${cartResourceUrl}`,
    {
      productId: productId,
      userId: user.profile.sub,
      quantity: 1,
    },
    getRequestOptions(user.access_token)
  );
  console.log(response);
  return response.data as ICart;
};

export const updateCartForCurrentUser = async (
  cartId: string,
  productId: string,
  quantity: number
) => {
  const user = await AuthService.getUser();
  const response: AxiosResponse = await axios.put<ICart>(
    `${cartResourceUrl}`,
    {
      productId: productId,
      quantity: quantity,
    },
    getRequestOptions(user.access_token)
  );
  console.log(response);
  return response.data as ICart;
};

export const deleteCartForCurrentUser = async (
  cartId: string,
  productId: string
) => {
  const user = await AuthService.getUser();
  const response: AxiosResponse = await axios.delete<ICart>(
    `${cartResourceUrl}/${cartId}/items/${productId}`,
    getRequestOptions(user.access_token)
  );
  console.log(response);
  return response.data as string;
};

export const checkoutForCurrentUser = async () => {
  const user = await AuthService.getUser();
  const response: AxiosResponse = await axios.put<ICart>(
    `${cartResourceUrl}/checkout`,
    {},
    getRequestOptions(user.access_token)
  );
  console.log(response);
  return response.data as ICart;
};
