import '../App.scss';

// it might be a nice refactor to have this component accept props and render within the Products component.
function ProductCard() {
  return (
    <div className="product-card">
        <div className="product-id">Product Id: </div>
        <div className="product-name">Product Name: </div>
        <div className="inventory-quantity">Inventory Quantity: </div>
        <div className="ship-on-weekends">Ship on Weekends? </div>
        <div className="max-days">Max Business Days to Ship: </div>
    </div>
  );
}

export default ProductCard;