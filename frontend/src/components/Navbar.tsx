import { Link } from 'react-router-dom';

const Navbar = () => {
  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
      <div className="container-fluid">
        <Link className="navbar-brand" to="/">APS Industrial</Link>
        <div className="collapse navbar-collapse">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            <li className="nav-item"><Link className="nav-link" to="/">Painel</Link></li>
            <li className="nav-item"><Link className="nav-link" to="/products">Produtos</Link></li>
            <li className="nav-item"><Link className="nav-link" to="/machines">Máquinas</Link></li>
            <li className="nav-item"><Link className="nav-link" to="/materials">Materiais</Link></li>
            <li className="nav-item"><Link className="nav-link" to="/bom">BOM</Link></li>
            <li className="nav-item"><Link className="nav-link" to="/planning">Planejamento</Link></li>
            <li className="nav-item"><Link className="nav-link" to="/tree">Árvore</Link></li>
            <li className="nav-item"><Link className="nav-link" to="/graph">Grafo</Link></li>
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;