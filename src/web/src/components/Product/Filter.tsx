import React from 'react'
import { Button, Input, Label } from 'reactstrap'
import styled from 'styled-components'

const StyledProductSideBar = styled.div`
  background-color: #fff;
  border: 1px solid #e6e6f2;
`

const StyledProductSideBarWidget = styled.div`
  border-bottom: 1px solid #e6e6f2;
  padding: 10px 20px;
  margin-bottom: 10px;
  &:last-child {
    border: 0px;
  }
`

const StyledProductSideBarWidgetTitle = styled.h4`
  font-size: 16px;
  margin-bottom: 10px;
`

const Filter: React.FC = () => {
  return (
    <>
      <StyledProductSideBar>
        <StyledProductSideBarWidget>
          <h4 className="mb-0">Filters</h4>
        </StyledProductSideBarWidget>

        <StyledProductSideBarWidget>
          <StyledProductSideBarWidgetTitle>Price</StyledProductSideBarWidgetTitle>
          <div className="custom-control custom-checkbox">
            <Label check>
              <Input type="radio" name="price" /> {`less than 100$`}
            </Label>
          </div>
          <div className="custom-control custom-checkbox">
            <Label check>
              <Input type="radio" name="price" /> {`from 100$ to 1000$`}
            </Label>
          </div>
          <div className="custom-control custom-checkbox">
            <Label check>
              <Input type="radio" name="price" /> {`greater than 1000$`}
            </Label>
          </div>
        </StyledProductSideBarWidget>

        <StyledProductSideBarWidget>
          <StyledProductSideBarWidgetTitle>Category</StyledProductSideBarWidgetTitle>
          <div className="custom-control custom-checkbox">
            <Input type="checkbox" className="custom-control-input" id="cat-1" />
            <Label className="custom-control-label" for="cat-1">
              Categories #1
            </Label>
          </div>
          <div className="custom-control custom-checkbox">
            <Input type="checkbox" className="custom-control-input" id="cat-2" />
            <Label className="custom-control-label" for="cat-2">
              Categories #2
            </Label>
          </div>
          <div className="custom-control custom-checkbox">
            <Input type="checkbox" className="custom-control-input" id="cat-3" />
            <Label className="custom-control-label" for="cat-3">
              Categories #3
            </Label>
          </div>
          <div className="custom-control custom-checkbox">
            <Input type="checkbox" className="custom-control-input" id="cat-4" />
            <Label className="custom-control-label" for="cat-4">
              Categories #4
            </Label>
          </div>
          <div className="custom-control custom-checkbox">
            <Input type="checkbox" className="custom-control-input" id="cat-5" />
            <Label className="custom-control-label" for="cat-5">
              Categories #5
            </Label>
          </div>
        </StyledProductSideBarWidget>

        <StyledProductSideBarWidget>
          <Button color="secondary">Reset Filter</Button>
        </StyledProductSideBarWidget>
      </StyledProductSideBar>
    </>
  )
}

export default Filter
