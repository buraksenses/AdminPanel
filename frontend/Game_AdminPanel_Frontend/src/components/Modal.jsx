import { useConfig } from "../Contexts/ConfigurationsContext";
import { BuildingTypeLabels } from "../enums/enums.js";
import styles from "./Modal.module.css";

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
    availableBuildingTypes,
  } = useConfig();

  const handleCloseModal = () => {
    setBuildingType("");
    setBuildingCost("");
    setConstructionTime("");
    setShowModal(false);
  };

  return (
    <div className={styles.modal}>
      <div className={styles.modalContent}>
        <h2>
          {modalType === "add"
            ? "Add Building Configuration"
            : "Update Building Configuration"}
        </h2>
        {error && <div className={styles.error}>{error}</div>}
        {modalType === "add" && (
          <div className={styles.formGroup}>
            <label>Building Type</label>
            <select
              value={buildingType}
              onChange={(e) => setBuildingType(parseInt(e.target.value))}
            >
              <option value="">Select...</option>
              {availableBuildingTypes.map((type) => (
                <option key={type.type} value={type.type}>
                  {BuildingTypeLabels[type.type]}
                </option>
              ))}
            </select>
          </div>
        )}
        <div className={styles.formGroup}>
          <label>Building Cost</label>
          <input
            type="number"
            value={buildingCost}
            onChange={(e) => setBuildingCost(e.target.value)}
          />
        </div>
        <div className={styles.formGroup}>
          <label>Construction Time (seconds)</label>
          <input
            type="number"
            value={constructionTime}
            onChange={(e) => setConstructionTime(e.target.value)}
          />
        </div>
        <div className={styles.formActions}>
          {modalType === "add" ? (
            <button onClick={handleAddConfiguration}>OK</button>
          ) : (
            <button onClick={handleUpdateConfiguration}>Update</button>
          )}
          <button onClick={() => handleCloseModal()}>Cancel</button>
        </div>
      </div>
    </div>
  );
}

export default Modal;
