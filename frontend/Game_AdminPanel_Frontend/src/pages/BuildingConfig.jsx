import { useState } from "react";
import "../ConfigurationPage.css";

const buildingTypes = [
  { type: "Farm", icon: "../../public/farm.png" },
  { type: "Academy", icon: "../../public/academy.png" },
  { type: "Headquarters", icon: "../../public/headquarters.png" },
  { type: "LumberMill", icon: "../../public/lumbermill.png" },
  { type: "Barracks", icon: "../../public/barracks.png" },
];

const ConfigurationPage = () => {
  const [configurations, setConfigurations] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [buildingType, setBuildingType] = useState("");
  const [buildingCost, setBuildingCost] = useState("");
  const [constructionTime, setConstructionTime] = useState("");
  const [error, setError] = useState("");

  const handleAddConfiguration = () => {
    if (
      !buildingType ||
      buildingCost <= 0 ||
      constructionTime < 30 ||
      constructionTime > 1800
    ) {
      setError("Invalid input. Please check the values.");
      return;
    }

    const newConfiguration = { buildingType, buildingCost, constructionTime };
    setConfigurations([...configurations, newConfiguration]);
    setShowModal(false);
    setBuildingType("");
    setBuildingCost("");
    setConstructionTime("");
    setError("");
  };

  const availableBuildingTypes = buildingTypes.filter(
    (type) =>
      !configurations.find((config) => config.buildingType === type.type)
  );

  return (
    <div className="configuration-page">
      <h1>Building Configuration</h1>
      <button className="add-button" onClick={() => setShowModal(true)}>
        Add Configuration
      </button>

      <div className="grid">
        {configurations.map((config, index) => (
          <div className="grid-item" key={index}>
            <img
              src={
                buildingTypes.find((b) => b.type === config.buildingType).icon
              }
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

      {showModal && (
        <div className="modal">
          <div className="modal-content">
            <h2>Add Building Configuration</h2>
            {error && <div className="error">{error}</div>}
            <div className="form-group">
              <label>Building Type</label>
              <select
                value={buildingType}
                onChange={(e) => setBuildingType(e.target.value)}
              >
                <option value="">Select...</option>
                {availableBuildingTypes.map((type, index) => (
                  <option key={index} value={type.type}>
                    {type.type}
                  </option>
                ))}
              </select>
            </div>
            <div className="form-group">
              <label>Building Cost</label>
              <input
                type="number"
                value={buildingCost}
                onChange={(e) => setBuildingCost(e.target.value)}
              />
            </div>
            <div className="form-group">
              <label>Construction Time (seconds)</label>
              <input
                type="number"
                value={constructionTime}
                onChange={(e) => setConstructionTime(e.target.value)}
              />
            </div>
            <div className="form-actions">
              <button onClick={handleAddConfiguration}>OK</button>
              <button onClick={() => setShowModal(false)}>Cancel</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ConfigurationPage;
