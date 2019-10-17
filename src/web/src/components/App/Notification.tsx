import React, { memo } from 'react'
import { Container, UncontrolledAlert } from 'reactstrap'
import { useStore } from 'stores/store'

const Notification = () => {
  const { state } = useStore()

  return (
    <>
      {state.isShowNotification && (
        <Container fluid>
          <UncontrolledAlert color="info">{state.notificationMessage}</UncontrolledAlert>
        </Container>
      )}
    </>
  )
}

export default memo(Notification)
