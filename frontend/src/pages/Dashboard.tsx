const Dashboard = () => {
  return (
    <div className="container mt-4">
      <h1>Painel</h1>
      <p>Bem-vindo ao Industrial APS!</p>
      <div className="row">
        <div className="col-md-4">
          <div className="card text-white bg-primary mb-3">
            <div className="card-header">Produtos</div>
            <div className="card-body">
              <h5 className="card-title">0</h5>
              <p className="card-text">Produtos cadastrados</p>
            </div>
          </div>
        </div>
        <div className="col-md-4">
          <div className="card text-white bg-success mb-3">
            <div className="card-header">Máquinas</div>
            <div className="card-body">
              <h5 className="card-title">0</h5>
              <p className="card-text">Máquinas disponíveis</p>
            </div>
          </div>
        </div>
        <div className="col-md-4">
          <div className="card text-white bg-warning mb-3">
            <div className="card-header">Ordens</div>
            <div className="card-body">
              <h5 className="card-title">0</h5>
              <p className="card-text">Ordens de produção</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;