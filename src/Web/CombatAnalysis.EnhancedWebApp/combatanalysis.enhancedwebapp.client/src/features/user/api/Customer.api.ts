import type { CustomerModel } from '../types/CustomerModel';
import { UserApi } from './User.api';

export const CustomerApi = UserApi.injectEndpoints({
    endpoints: builder => ({
        getCustomerById: builder.query<CustomerModel, string>({
            query: id => `/Customer/${id}`,
            providesTags: result => result ? [{ type: 'Customer', id: result.id }] : [],
        }),
        createCustomer: builder.mutation<CustomerModel, CustomerModel>({
            query: customer => ({
                body: customer,
                url: '/Customer',
                method: 'POST'
            }),
            invalidatesTags: result => result ? [{ type: 'Customer', id: result.id }] : [],
        }),
        editCustomer: builder.mutation<void, CustomerModel>({
            query: customer => ({
                body: customer,
                url: '/Customer',
                method: 'PUT'
            }),
            invalidatesTags: (_result, _error, customer) => [{ type: 'Customer', id: customer.id }],
        }),
        findCustomerByUserId: builder.query<CustomerModel, string>({
            query: id => `/Customer/findByUserId/${id}`,
            providesTags: (_result, _error, id) => [{ type: 'Customer', id }],
        }),
    })
})

export const {
    useGetCustomerByIdQuery,
    useCreateCustomerMutation,
    useLazyGetCustomerByIdQuery,
    useEditCustomerMutation,
    useLazyFindCustomerByUserIdQuery
} = CustomerApi;