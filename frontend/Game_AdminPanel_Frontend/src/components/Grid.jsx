function Grid({ configurations, buildingTypes }) {
  return (
    <div className="grid">
      {configurations.map((config, index) => (
        <div className="grid-item" key={index}>
          <img
            src={buildingTypes.find((b) => b.type === config.buildingType).icon}
            alt={config.buildingType}
          />
          <div className="grid-item-details">
            <h3>{config.buildingType}</h3>
            <p>Cost: {config.buildingCost}</p>
            <p>Time: {config.constructionTime}s</p>
          </div>
        </div>
      ))}
    </div>
  );
}

export default Grid;
