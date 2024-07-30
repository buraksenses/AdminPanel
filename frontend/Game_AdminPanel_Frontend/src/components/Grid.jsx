import { useConfig } from "../Contexts/ConfigurationsContext";
import {BuildingTypeLabels} from "../enums/Enums.js";

function Grid() {
  const {
    configurations,
    buildingTypes,
    openUpdateModal,
    handleRemoveConfiguration,
  } = useConfig();

  console.log("Configurations:", configurations);
  console.log("BuildingTypes:", buildingTypes);

  return (
      <div className="grid">
        {configurations.map((config, index) => {
          const buildingTypeObj = buildingTypes.find((b) => b.type === config.buildingType);
          console.log("Config:", config);
          console.log("BuildingTypeObj:", buildingTypeObj);

          if (!buildingTypeObj) {
            console.error(`Building type ${config.buildingType} not found in buildingTypes array.`);
          }

          const icon = buildingTypeObj ? buildingTypeObj.icon : null;

          return (
              <div className="grid-item" key={index}>
                {icon && <img src={icon} alt={BuildingTypeLabels[config.buildingType]}/>}
                <div className="grid-item-details">
                  <h3>{BuildingTypeLabels[config.buildingType]}</h3>
                  <p>Cost: {config.buildingCost}</p>
                  <p>Time: {config.constructionTime}s</p>
                </div>
                <div className="overlay">
                  <button onClick={() => openUpdateModal(config)}>Update</button>
                  <button onClick={() => handleRemoveConfiguration(config)}>Remove</button>
                </div>
              </div>
          );
        })}
      </div>
  );
}

export default Grid;
