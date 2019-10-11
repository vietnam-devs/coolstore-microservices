import React from 'react'
import { Button } from 'reactstrap'

const Filter: React.FC = () => {
  return (
    <>
      <div className="product-sidebar">
        <div className="product-sidebar-widget">
          <h4 className="mb-0">Filters</h4>
        </div>
        <div className="product-sidebar-widget">
          <h4 className="product-sidebar-widget-title">Category</h4>
          <div className="custom-control custom-checkbox">
            <input type="checkbox" className="custom-control-input" id="cat-1" />
            <label className="custom-control-label">Categories #1</label>
          </div>
          <div className="custom-control custom-checkbox">
            <input type="checkbox" className="custom-control-input" id="cat-2" />
            <label className="custom-control-label">Categories #2</label>
          </div>
          <div className="custom-control custom-checkbox">
            <input type="checkbox" className="custom-control-input" id="cat-3" />
            <label className="custom-control-label">Categories #3</label>
          </div>
          <div className="custom-control custom-checkbox">
            <input type="checkbox" className="custom-control-input" id="cat-4" />
            <label className="custom-control-label">Categories #4</label>
          </div>
          <div className="custom-control custom-checkbox">
            <input type="checkbox" className="custom-control-input" id="cat-5" />
            <label className="custom-control-label">Categories #5</label>
          </div>
        </div>
        <div className="product-sidebar-widget">
          <h4 className="product-sidebar-widget-title">Price</h4>
          <div className="custom-control custom-checkbox">
            <input type="checkbox" className="custom-control-input" id="price-1" />
            <label className="custom-control-label">$$</label>
          </div>
          <div className="custom-control custom-checkbox">
            <input type="checkbox" className="custom-control-input" id="price-2" />
            <label className="custom-control-label">$$$</label>
          </div>
          <div className="custom-control custom-checkbox">
            <input type="checkbox" className="custom-control-input" id="price-3" />
            <label className="custom-control-label">$$$$</label>
          </div>
        </div>
        <div className="product-sidebar-widget">
          <Button color="secondary">Reset Filter</Button>
        </div>
      </div>
    </>
  )
}

export default Filter
