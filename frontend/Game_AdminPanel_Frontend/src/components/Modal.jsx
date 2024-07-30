import { useConfig } from "../Contexts/ConfigurationsContext";
import {BuildingType, BuildingTypeLabels} from "../enums/Enums.js";

function Modal() {
  const {
    modalType,
    error,
    buildingType,
    setBuildingType,
    buildingCost,
    setBuildingCost,
    constructionTime,
    setConstructionTime,
    handleAddConfiguration,
    handleUpdateConfiguration,
    setShowModal,
  } = useConfig();

  return (
      <div className="modal">
        <div className="modal-content">
          <h2>
            {modalType === "add"
                ? "Add Building Configuration"
                : "Update Building Configuration"}
          </h2>
          {error && <div className="error">{error}</div>}
          {modalType === "add" && (
              <div className="form-group">
                <label>Building Type</label>
                <select value={buildingType} onChange={(e) => setBuildingType(parseInt(e.target.value))}>
                  <option value="">Select...</option>
                  {Object.entries(BuildingTypeLabels).map(([key, label]) => (
                      <option key={key} value={key}>
                        {label}
                      </option>
                  ))}
                </select>
              </div>
          )}
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
            {modalType === "add" ? (
                <button onClick={handleAddConfiguration}>OK</button>
            ) : (
                <button onClick={handleUpdateConfiguration}>Update</button>
            )}
            <button onClick={() => setShowModal(false)}>Cancel</button>
          </div>
        </div>
      </div>
  );
}

export default Modal;
