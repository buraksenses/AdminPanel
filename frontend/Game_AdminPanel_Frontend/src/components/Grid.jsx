import { useConfig } from "../Contexts/ConfigurationsContext";

function Grid() {
  const {
    configurations,
    buildingTypes,
    openUpdateModal,
    handleRemoveConfiguration,
  } = useConfig();

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
          <div className="overlay">
            <button onClick={() => openUpdateModal(config)}>Update</button>
            <button onClick={() => handleRemoveConfiguration(config)}>
              Remove
            </button>
          </div>
        </div>
      ))}
    </div>
  );
}

export default Grid;
