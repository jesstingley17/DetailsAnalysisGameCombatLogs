import type { CustomerModel } from '../types/CustomerModel';
import { UserApi } from './User.api';

export const CustomerApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        getCustomerById: builder.query<CustomerModel, string>({
            query: id => `/Customer/${id}`,
        }),
        createCustomer: builder.mutation<CustomerModel, CustomerModel>({
            query: customer => ({
                body: customer,
                url: '/Customer',
                method: 'POST'
            })
        }),
        editCustomer: builder.mutation<void, CustomerModel>({
            query: customer => ({
                body: customer,
                url: '/Customer',
                method: 'PUT'
            })
        }),
        searchCustomerByUserId: builder.query<CustomerModel, string>({
            query: id => `/Customer/searchByUserId/${id}`,
            providesTags: (result, error, id) => [{ type: 'Customer', id }],
        }),
    })
})

export const {
    useGetCustomerByIdQuery,
    useCreateCustomerMutation,
    useLazyGetCustomerByIdQuery,
    useEditCustomerMutation,
    useLazySearchCustomerByUserIdQuery
} = CustomerApi;