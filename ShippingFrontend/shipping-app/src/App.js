import logo from './logo.svg';
import './App.scss';
import LoadingSpinner from './components/LoadingSpinner';
import ProductCard from './components/ProductCard';
import Products from './components/Products';
import axios from './axios'

function App() {
  return (
    <div className="main-grid">
      <div className="title-section">Shipping Products</div>
      {/* <LoadingSpinner /> */}
      <Products />
    </div>
  );
}

export default App;
