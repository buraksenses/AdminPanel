import { createContext, useContext, useEffect, useState } from "react";
import farmIcon from "../../public/farm.png";
import lumbermillIcon from "../../public/lumbermill.png";
import barracksIcon from "../../public/barracks.png";
import academyIcon from "../../public/academy.png";
import headquartersIcon from "../../public/headquarters.png";
import {BuildingType, BuildingTypeLabels, BuildingTypeStringToInt} from "../enums/Enums.js";
import apiClient from "../api/GameApiService.jsx";
import {showInfoToast, showSuccessToast, showWarningToast} from "../utils/notifications.js";
import {useAuth} from "../security/AuthContext.jsx";

const READ_BASE_URL = "http://localhost:5129";
const WRITE_BASE_URL = "http://localhost:5228";

const buildingTypes = [
  { type: BuildingType.Farm, icon: farmIcon },
  { type: BuildingType.Academy, icon: academyIcon },
  { type: BuildingType.HeadQuarters, icon: headquartersIcon },
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
  const [confetti, setConfetti] = useState(false);
  const [newConfigIndex, setNewConfigIndex] = useState(null);
  const {setIsAuthenticated, setIsSessionExpired} = useAuth();

  useEffect(() => {
    if (!showModal && confetti) {
      setConfetti(false);
    }
  }, [showModal, configurations, confetti]);

  const triggerConfetti = () => {
    setConfetti(true);
    setTimeout(() => setConfetti(false), 2000);
  };

  useEffect(() => {
    if (newConfigIndex !== null) {
      const timer = setTimeout(() => {
        setNewConfigIndex(null);
      }, 1000);

      return () => clearTimeout(timer);
    }
  }, [newConfigIndex]);


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
      const response = await apiClient.post(
          `${WRITE_BASE_URL}/api/buildings`,
          newConfiguration
      );

      if (response.data.statusCode === 201) {
        setConfigurations([...configurations, response.data.data]);
        setNewConfigIndex(configurations.length);
        setShowModal(false);
        setBuildingType("");
        setBuildingCost("");
        setConstructionTime("");
        setError("");
        setTimeout(() => triggerConfetti(), 500);
        showSuccessToast(`${BuildingTypeLabels[buildingType]} configuration added! Values: cost:${buildingCost} , constructionTime:${constructionTime}`);
      } else {
        setError("Failed to add configuration. Please try again.");
      }
    } catch (error) {
      console.log(error.message);

      if (checkSessionExpired(error))
        return;

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
      const response = await apiClient.put(`${WRITE_BASE_URL}/api/buildings`, {
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
        showSuccessToast(`${BuildingTypeLabels[buildingType]} configuration updated! Values: cost:${buildingCost} , constructionTime:${constructionTime}`);
      }
    } catch (error) {
      if (error.response && error.response.status === 401) {
        console.log("Token expired. Redirecting to login.");
        window.location.href = '/auth';
        return;
      }

      if (checkSessionExpired(error))
        return;

      setError("Failed to update configuration. Please try again.");
      //alert(`There was an error updating the configuration: ${error.message}`);
    }
  };

  const handleRemoveConfiguration = async (configToRemove) => {


    try {
      const response = await apiClient.delete(`${WRITE_BASE_URL}/api/buildings/${configToRemove.id}`);

      if (response.data.statusCode === 200) {
        const updatedConfigurations = configurations.filter(
            (config) => config.id !== configToRemove.id
        );
        setConfigurations(updatedConfigurations);
        showSuccessToast(`${BuildingTypeLabels[buildingType]} configuration removed! Values: cost:${buildingCost} , constructionTime:${constructionTime}`);
      }
    } catch (error) {
      console.log(error.message)

      if (checkSessionExpired(error))
        return;

      setError("Failed to remove configuration. Please try again.");
      alert(`There was an error removing the configuration: ${error.message}`);
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

  const reset = () => {
    setShowModal(false);
    setModalType('');
    setBuildingCost('');
    setConstructionTime('');
  }

  const checkSessionExpired = (error) => {
    if (error.message === 'Session expired!') {
      showWarningToast("Your session has expired!")
      setIsAuthenticated(false);
      reset();
      return true;
    }
    return false;
  }

  return (
    <ConfigurationsContext.Provider
      value={{
        configurations,
        showModal,
        modalType,
        buildingType,
        buildingTypes,
        availableBuildingTypes,
        buildingCost,
        constructionTime,
        selectedConfig,
        error,
        isLoading,
        confetti,
        newConfigIndex,
        triggerConfetti,
        handleAddConfiguration,
        handleUpdateConfiguration,
        handleRemoveConfiguration,
        openUpdateModal,
        reset,
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
