import { useConfig } from "../Contexts/ConfigurationsContext";
import { BuildingTypeLabels } from "../enums/enums.js";
import styles from "./Grid.module.css";
import Spinner from "./Spinner.jsx";

function Grid() {
  const {
    configurations,
    buildingTypes,
    openUpdateModal,
    handleRemoveConfiguration,
    setModalType,
    setShowModal,
    newConfigIndex,
    isLoading,
  } = useConfig();

  const handleAddNewConfiguration = () => {
    setModalType("add");
    setShowModal(true);
  };

  return (
    <div className={styles.gridContainer}>
      {isLoading && <Spinner />}
      <div className={styles.grid}>
        {configurations.map((config, index) => {
          const buildingTypeObj = buildingTypes.find(
            (b) => b.type === config.buildingType
          );

          if (!buildingTypeObj) {
            console.error(
              `Building type ${config.buildingType} not found in buildingTypes array.`
            );
          }

          const icon = buildingTypeObj ? buildingTypeObj.icon : null;

          const itemClass =
            index === newConfigIndex
              ? `${styles.gridItem} ${styles.newItemAnimation}`
              : `${styles.gridItem}`;

          return (
            <div className={itemClass} key={index}>
              {icon && (
                <img src={icon} alt={BuildingTypeLabels[config.buildingType]} />
              )}
              <div className={styles.gridItemDetails}>
                <h3>{BuildingTypeLabels[config.buildingType]}</h3>
                <p>Cost: {config.buildingCost}</p>
                <p>Time: {config.constructionTime}s</p>
              </div>
              <div className={styles.overlay}>
                <button onClick={() => openUpdateModal(config)}>Update</button>
                <button onClick={() => handleRemoveConfiguration(config)}>
                  Remove
                </button>
              </div>
            </div>
          );
        })}
        <div
          className={`${styles.gridItem} ${styles.addItem}`}
          onClick={handleAddNewConfiguration}
        >
          <div className={styles.plusSign}>+</div>
        </div>
      </div>
    </div>
  );
}

export default Grid;
