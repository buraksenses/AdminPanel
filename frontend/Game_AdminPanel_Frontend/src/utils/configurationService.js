import apiClient from "../api/GameApiService.jsx";
import {showSuccessToast} from "./notifications.js";
import {BuildingType, BuildingTypeLabels, BuildingTypeStringToInt} from "../enums/enums.js";
import {readBaseURL, writeBaseURL} from "./config.js";

export const addConfiguration = async (configurations,
                                       setConfigurations,
                                       setNewConfigIndex,
                                       buildingType,
                                       setError,
                                       checkSessionExpired,
                                       buildingCost,
                                       constructionTime,
                                       reset) => {
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
            `${writeBaseURL}/api/buildings`,
            newConfiguration
        );

        if (response.data.statusCode === 201) {
            setConfigurations([...configurations, response.data.data]);
            setNewConfigIndex(configurations.length);
            showSuccessToast(`${BuildingTypeLabels[buildingType]} configuration added! Values: cost:${buildingCost} , constructionTime:${constructionTime}`);
            reset();
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
}

export const updateConfiguration = async(buildingCost, buildingType, constructionTime, selectedConfig, configurations
,setError, setConfigurations, reset, checkSessionExpired) => {
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
        const response = await apiClient.put(`${writeBaseURL}/api/buildings`, {
            id: selectedConfig.id,
            buildingCost,
            constructionTime,
        });

        console.log(response.data);

        if (response.data.statusCode === 200) {
            setConfigurations(updatedConfigurations);
            reset();
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
}

export const removeConfiguration = async(
    configurations,
    configToRemove,
    setConfigurations,
    checkSessionExpired,
    setError,
    buildingType,
    buildingCost,
    constructionTime
) => {
    try {
        const response = await apiClient.delete(`${writeBaseURL}/api/buildings/${configToRemove.id}`);

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
}

export const getConfigurations = async(isLogin,
                                         setIsLoading,
                                         setConfigurations) => {
    if(!isLogin)
        return;
    try {
        setIsLoading(true);
        const res = await fetch(`${readBaseURL}/api/buildings`);
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