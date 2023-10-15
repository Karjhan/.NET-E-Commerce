import { Address } from "./user"

export interface OrderItem{
    productId: number,
    productName: string,
    pictureURL: string,
    price: number,
    quantity: number
}

export interface OrderToCreate{
    basketId: string,
    deliveryMethodId: number,
    shipToAddress: Address
}

export interface Order{
    id: number,
    buyerEmail: string,
    orderDate: Date,
    shipToAddress: Address,
    deliveryMethod: string,
    shippingPrice: number,
    orderItems: OrderItem[],
    subtotal: number,
    total: number,
    status: string
}