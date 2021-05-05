import React, {Component} from 'react'
import axios from '../axios'

export default class Products extends Component {
    constructor(props) {
        super(props);
        this.state = {
            products: []
        };
    }
    getProductsData() {
        axios
            .get(`/products/getall`, {})
            .then(res => {
                const products = res.data;
                console.log(products);

                this.setState({products});
                console.log(this.state.products)
                
            })
            .catch((error) => {
                console.log(error)
            })

    }
    componentDidMount(){
        this.getProductsData()
    }
    render() {

        return (
            <div className="product-section">
                {this.state.products.map(function(product, index){

                    return (
                        <div className="product-card">
                            {/* <div className="product-id">Product Id: </div> */}
                            <div className="product-name">{product.productName}</div>
                            <div className="inventory-quantity">Inventory Quantity: {product.inventoryQuantity}</div>
                            {/* <div className="ship-on-weekends">Ship on Weekends? </div> */}
                            <div className="max-days">Ship Date: {product.shipDate.toString()}</div>
                        </div>
                    )
                })}
            </div>
        )
    }
}