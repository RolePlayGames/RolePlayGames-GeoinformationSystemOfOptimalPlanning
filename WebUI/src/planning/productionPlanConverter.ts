import { Task } from "gantt-task-react";
import { ProductionPlanInfo } from "./planningClient";

export const convertProductionPlanToTasks = (plan: ProductionPlanInfo) => {
	const tasks: Task[] = [];
	let lastTaskId: string | null = null;

	for (const lineQueue of plan.productionLineQueues) {
		const projectTask: Task = {
			id: lineQueue.productionLineName,
			name: lineQueue.productionLineName,
			start: lineQueue.orderPositions.length > 0 ? new Date(lineQueue.orderPositions[0].orderProductionStartDateTime) : new Date(),
			end: lineQueue.orderPositions.length > 0 ? new Date(lineQueue.orderPositions[lineQueue.orderPositions.length - 1].orderProductionEndDateTime) : new Date(), 
			progress: 100,
			type: 'project',
			hideChildren: false,
		};

		tasks.push(projectTask);
		const projectTaskId = projectTask.id;

		for (const order of lineQueue.orderPositions) {
			const orderTask: Task = {
				id: order.orderNumber,
				name: order.orderNumber,
				start: new Date(order.orderProductionStartDateTime),
				end: new Date(order.orderProductionEndDateTime),
				progress: 0,
				type: 'task',
				dependencies: lastTaskId ? [lastTaskId] : [],
				project: projectTaskId,
			};

			tasks.push(orderTask);
			lastTaskId = orderTask.id;
		}
        
		lastTaskId = null;
	}

	return tasks;
}