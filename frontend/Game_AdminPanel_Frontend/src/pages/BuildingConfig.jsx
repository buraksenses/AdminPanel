import { useState } from "react";
import "../ConfigurationPage.css";
import Modal from "../components/Modal";
import Grid from "../components/Grid";

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

      <Grid configurations={configurations} buildingTypes={buildingTypes} />

      {showModal && (
        <Modal
          error={error}
          buildingCost={buildingCost}
          buildingType={buildingType}
          availableBuildingTypes={availableBuildingTypes}
          setBuildingCost={setBuildingCost}
          setBuildingType={setBuildingType}
          constructionTime={constructionTime}
          setConstructionTime={setConstructionTime}
          handleAddConfiguration={handleAddConfiguration}
          setShowModal={setShowModal}
        />
      )}
    </div>
  );
};

export default ConfigurationPage;
