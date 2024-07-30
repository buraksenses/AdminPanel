import { createContext, useContext, useEffect, useState } from "react";
import farmIcon from "../../public/farm.png";
import lumbermillIcon from "../../public/lumbermill.png";
import barracksIcon from "../../public/barracks.png";
import academyIcon from "../../public/academy.png";
import headquartersIcon from "../../public/headquarters.png";
import axios from "axios";
import {BuildingType, BuildingTypeStringToInt} from "../enums/Enums.js";

const READ_BASE_URL = "http://localhost:5129";
const WRITE_BASE_URL = "http://localhost:5228";

const buildingTypes = [
  { type: BuildingType.Farm, icon: farmIcon },
  { type: BuildingType.Academy, icon: academyIcon },
  { type: BuildingType.Headquarters, icon: headquartersIcon },
  { type: BuildingType.LumberMill, icon: lumbermillIcon },
  { type: BuildingType.Barracks, icon: barracksIcon },
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
  const [isLoading, setIsLoading] = useState(false);

  useEffect(function () {
    async function fetchConfigurations() {
      try {
        setIsLoading(true);
        const res = await fetch(`${READ_BASE_URL}/api/buildings`);
        const data = await res.json();
        console.log(data);

        const updatedConfigurations = data.data.map(config => ({
          ...config,
          buildingType: BuildingTypeStringToInt[config.buildingType] || config.buildingType,
        }));

        setConfigurations(updatedConfigurations);
      } catch (error) {
        alert(error.response.data.errors);
      } finally {
        setIsLoading(false);
      }
    }
    fetchConfigurations();
  }, []);

  const handleAddConfiguration = async () => {
    if (
        !buildingType ||
        !Object.values(BuildingType).includes(parseInt(buildingType)) ||
        buildingCost <= 0 ||
        constructionTime < 30 ||
        constructionTime > 1800
    ) {
      setError("Invalid input. Please check the values.");
      return;
    }

    const newConfiguration = { buildingType: parseInt(buildingType), buildingCost, constructionTime };


    try {
      const response = await axios.post(
          `${WRITE_BASE_URL}/api/buildings`,
          newConfiguration
      );

      if (response.data.statusCode === 201) {
        setConfigurations([...configurations, newConfiguration]);
        setShowModal(false);
        setBuildingType("");
        setBuildingCost("");
        setConstructionTime("");
        setError("");
      } else {
        setError("Failed to add configuration. Please try again.");
      }
    } catch (error) {
      setError(error.message);
      alert(error.message);
    }
  };

  const handleUpdateConfiguration = async () => {
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

    try {
      const response = await axios.put(`${WRITE_BASE_URL}/api/buildings`, {
        id: selectedConfig.id,
        buildingCost,
        constructionTime,
      });

      console.log(response.data);

      if (response.data.statusCode === 200) {
        setConfigurations(updatedConfigurations);
        setShowModal(false);
        setSelectedConfig(null);
        setBuildingCost("");
        setConstructionTime("");
        setError("");
      }
    } catch (error) {
      setError("Failed to update configuration. Please try again.");
      alert("There was an error updating the configuration:", error);
    }
  };

  const handleRemoveConfiguration = async (configToRemove) => {
    const updatedConfigurations = configurations.filter(
      (config) => config !== configToRemove
    );

    try {
      const response = await axios.delete(`${WRITE_BASE_URL}/api/buildings`);

      if (response.data.StatusCode === 200) {
        setConfigurations(updatedConfigurations);
      }
    } catch (error) {
      setError("Failed to remove configuration. Please try again.");
      alert("There was an error removing the configuration:", error);
    }
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
