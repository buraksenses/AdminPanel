import { createContext, useContext, useEffect, useState } from "react";
import farmIcon from "../../public/farm.png";
import lumbermillIcon from "../../public/lumbermill.png";
import barracksIcon from "../../public/barracks.png";
import academyIcon from "../../public/academy.png";
import headquartersIcon from "../../public/headquarters.png";
import {BuildingType} from "../enums/enums.js";
import {showWarningToast} from "../utils/notifications.js";
import {useAuth} from "./AuthContext.jsx";
import {
  addConfiguration,
  getConfigurations,
  removeConfiguration,
  updateConfiguration
} from "../utils/configurationService.js";

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
  const [newConfigIndex, setNewConfigIndex] = useState(null);
  const {setIsAuthenticated, isLogin} = useAuth();

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
      await getConfigurations(isLogin, setIsLoading, setConfigurations);
    }
    fetchConfigurations();
  }, [isLogin]);

  const handleAddConfiguration = async () => {
    await addConfiguration(configurations, setConfigurations, setNewConfigIndex, buildingType
    ,setError, checkSessionExpired, buildingCost, constructionTime, reset);
  };

  const handleUpdateConfiguration = async () => {
    await updateConfiguration(buildingCost,buildingType,constructionTime,
        selectedConfig,configurations,setError,setConfigurations,reset,checkSessionExpired);
  };

  const handleRemoveConfiguration = async (configToRemove) => {
    await removeConfiguration(configurations,configToRemove,setConfigurations,checkSessionExpired
    ,setError,buildingType,buildingCost,constructionTime);
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
    setBuildingType('');
    setError('');
    setSelectedConfig(null);
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
        newConfigIndex,
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
