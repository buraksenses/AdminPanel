function Modal({
  error,
  buildingType,
  availableBuildingTypes,
  buildingCost,
  setBuildingCost,
  setBuildingType,
  constructionTime,
  setConstructionTime,
  handleAddConfiguration,
  setShowModal,
  modalType,
  handleUpdateConfiguration,
}) {

  



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
