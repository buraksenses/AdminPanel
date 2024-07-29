import { useState } from "react";
import "../ConfigurationPage.css";
import Modal from "../components/Modal";
import farmIcon from "../../public/farm.png";
import lumbermillIcon from "../../public/lumbermill.png";
import barracksIcon from "../../public/barracks.png";
import academyIcon from "../../public/academy.png";
import headquartersIcon from "../../public/headquarters.png";
import Grid from "../components/Grid";

const buildingTypes = [
  { type: "Farm", icon: farmIcon },
  { type: "Academy", icon: academyIcon },
  { type: "Headquarters", icon: headquartersIcon },
  { type: "LumberMill", icon: lumbermillIcon },
  { type: "Barracks", icon: barracksIcon },
];

const ConfigurationPage = () => {
  const [configurations, setConfigurations] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [modalType, setModalType] = useState("");
  const [buildingType, setBuildingType] = useState("");
  const [buildingCost, setBuildingCost] = useState("");
  const [constructionTime, setConstructionTime] = useState("");
  const [selectedConfig, setSelectedConfig] = useState(null);
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

  const handleUpdateConfiguration = () => {
    if (
      !buildingCost ||
      buildingCost <= 0 ||
      constructionTime < 30 ||
      constructionTime > 1800
    ) {
      setError("Invalid input. Please check the values.");
      return;
    }

    const updatedConfigurations = configurations.map((config) =>
      config === selectedConfig
        ? { ...config, buildingCost, constructionTime }
        : config
    );
    setConfigurations(updatedConfigurations);
    setShowModal(false);
    setSelectedConfig(null);
    setBuildingCost("");
    setConstructionTime("");
    setError("");
  };

  const handleRemoveConfiguration = (configToRemove) => {
    const updatedConfigurations = configurations.filter(
      (config) => config !== configToRemove
    );
    setConfigurations(updatedConfigurations);
  };

  const openUpdateModal = (config) => {
    setSelectedConfig(config);
    setBuildingCost(config.buildingCost);
    setConstructionTime(config.constructionTime);
    setModalType("update");
    setShowModal(true);
  };

  const availableBuildingTypes = buildingTypes.filter(
    (type) =>
      !configurations.find((config) => config.buildingType === type.type)
  );

  return (
    <div className="configuration-page">
      <h1>Building Configuration</h1>
      <Grid
        configurations={configurations}
        buildingTypes={buildingTypes}
        openUpdateModal={openUpdateModal}
        handleRemoveConfiguration={handleRemoveConfiguration}
      />
      <button
        className="add-button"
        onClick={() => {
          setModalType("add");
          setShowModal(true);
        }}
      >
        Add Configuration
      </button>
      {showModal && (
        <Modal
          error={error}
          buildingType={buildingType}
          availableBuildingTypes={availableBuildingTypes}
          buildingCost={buildingCost}
          setBuildingCost={setBuildingCost}
          setBuildingType={setBuildingType}
          constructionTime={constructionTime}
          setConstructionTime={setConstructionTime}
          handleAddConfiguration={handleAddConfiguration}
          setShowModal={setShowModal}
          modalType={modalType}
          handleUpdateConfiguration={handleUpdateConfiguration}
        />
      )}
    </div>
  );
};

export default ConfigurationPage;
