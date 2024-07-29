import { createContext, useContext, useState } from "react";
import farmIcon from "../../public/farm.png";
import lumbermillIcon from "../../public/lumbermill.png";
import barracksIcon from "../../public/barracks.png";
import academyIcon from "../../public/academy.png";
import headquartersIcon from "../../public/headquarters.png";

const buildingTypes = [
  { type: "Farm", icon: farmIcon },
  { type: "Academy", icon: academyIcon },
  { type: "Headquarters", icon: headquartersIcon },
  { type: "LumberMill", icon: lumbermillIcon },
  { type: "Barracks", icon: barracksIcon },
];

const ConfigurationsContext = createContext();

function ConfigurationsProvider({ children }) {
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
    <ConfigurationsContext.Provider
      value={{
        configurations,
        showModal,
        modalType,
        buildingType,
        buildingTypes,
        buildingCost,
        constructionTime,
        selectedConfig,
        error,
        availableBuildingTypes,
        handleAddConfiguration,
        handleUpdateConfiguration,
        handleRemoveConfiguration,
        openUpdateModal,
        setModalType,
        setShowModal,
        setBuildingCost,
        setBuildingType,
        setConstructionTime,
        setConfigurations,
        setSelectedConfig,
      }}
    >
      {children}
    </ConfigurationsContext.Provider>
  );
}

function useConfig() {
  const context = useContext(ConfigurationsContext);
  if (context === undefined)
    throw new Error(
      "ConfigurationsContext was used outside the ConfigurationsProvider!"
    );
  return context;
}

export { useConfig, ConfigurationsProvider };
