import React, { useState } from 'react'
import { Button, Input, Label, CustomInput, Badge } from 'reactstrap'
import styled from 'styled-components'
import _ from 'lodash'

import { ICategoryTagModel, IInventoryTagModel } from 'stores/types'

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

interface IProps {
  initPrice: number
  maxPrice: number
  onPriceFilterChange: (price: number) => void
  categoryTags: ICategoryTagModel[]
  inventoryTags: IInventoryTagModel[]
}

const Filter: React.FC<IProps> = ({ onPriceFilterChange, initPrice, maxPrice, categoryTags, inventoryTags }) => {
  const [price, setPrice] = useState(initPrice)
  return (
    <>
      <StyledProductSideBar>
        <StyledProductSideBarWidget>
          <h4 className="mb-0">Filters</h4>
        </StyledProductSideBarWidget>

        <StyledProductSideBarWidget>
          <StyledProductSideBarWidgetTitle>Price</StyledProductSideBarWidgetTitle>
          <Label for="exampleCustomRange">${price.toFixed(2)}</Label>
          <CustomInput
            type="range"
            id="exampleCustomRange"
            name="customRange"
            min={0}
            defaultValue={initPrice}
            max={maxPrice}
            onMouseUp={e => {
              _.debounce(price => {
                onPriceFilterChange(price)
                setPrice(price)
              }, 100)(+e.currentTarget.value)
            }}
          />
        </StyledProductSideBarWidget>

        <StyledProductSideBarWidget>
          <StyledProductSideBarWidgetTitle>Category</StyledProductSideBarWidgetTitle>
          {categoryTags.map(category => (
            <div className="custom-control custom-checkbox">
              <Input type="checkbox" className="custom-control-input" id={category.key} checked />
              <Label className="custom-control-label" for={category.key}>
                {category.key} <Badge color="success">{category.count}</Badge>
              </Label>
            </div>
          ))}
        </StyledProductSideBarWidget>

        <StyledProductSideBarWidget>
          <StyledProductSideBarWidgetTitle>Location</StyledProductSideBarWidgetTitle>
          {inventoryTags.map(inventory => (
            <div className="custom-control custom-checkbox">
              <Input type="checkbox" className="custom-control-input" id={inventory.key} checked />
              <Label className="custom-control-label" for={inventory.key}>
                {inventory.key} <Badge color="success">{inventory.count}</Badge>
              </Label>
            </div>
          ))}
        </StyledProductSideBarWidget>

        <StyledProductSideBarWidget>
          <Button color="warning">Reset Filter</Button>
        </StyledProductSideBarWidget>
      </StyledProductSideBar>
    </>
  )
}

export default Filter
