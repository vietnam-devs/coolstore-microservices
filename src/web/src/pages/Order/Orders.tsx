import React, { useEffect, useCallback } from "react";
import { RouteComponentProps } from "react-router-dom";

import { withLayout } from "components/HOC";
import { Table, Badge } from "reactstrap";
import styled from "styled-components";

import { AppActions, useStore } from "stores/store";
import { getOrders } from "services/SaleService";

const MySpace = styled.div`
  margin: 2rem;
`;

interface IProps extends RouteComponentProps {}

const Orders: React.FC<IProps> = (props) => {
  const { state, dispatch } = useStore();

  const fetchData = useCallback(async () => {
    const result = await getOrders();
    dispatch(AppActions.loadOrders(result));
  }, [dispatch]);

  useEffect(() => {
    fetchData();
  }, [state.isProductsLoaded, fetchData]);

  return (
    <>
      <MySpace></MySpace>
      <Table striped>
        <thead>
          <tr>
            <th>Customer name</th>
            <th>Order date</th>
            <th>Complete date</th>
            <th>Number of products</th>
            <th>Order status</th>
          </tr>
        </thead>
        <tbody>
          {state.orders &&
            state.orders.length > 0 &&
            state.orders.map((order) => (
              <tr key={order.id}>
                <td>{order.customerFullName}</td>
                <td>{order.orderDate}</td>
                <td>{order.completeDate}</td>
                <td>
                  {order.orderItems && order.orderItems.length > 0
                    ? order.orderItems.length
                    : 0}
                </td>
                <td>
                  {order.orderStatus == 0 ? (
                    <Badge color="primary">Received</Badge>
                  ) : order.orderStatus == 1 ? (
                    <Badge color="warning">Processing</Badge>
                  ) : (
                    <Badge color="success">Completed</Badge>
                  )}
                </td>
              </tr>
            ))}
        </tbody>
      </Table>
    </>
  );
};

export default withLayout(Orders);
